import importlib.util
import subprocess
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
spec = importlib.util.spec_from_file_location("prepare_session", ROOT / "scripts/prepare-session.py")
module = importlib.util.module_from_spec(spec)
spec.loader.exec_module(module)


class SessionExportTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        self.source = Path(self.temp.name) / "source"
        self.source.mkdir()
        subprocess.run(["git", "init", "--quiet", str(self.source)], check=True)

    def test_export_keeps_working_sources_but_not_private_files_or_history(self):
        (self.source / "README.md").write_text("Demo project\n")
        (self.source / ".env.example").write_text("DEMO=placeholder\n")
        (self.source / ".env").write_text("DEMO=private-local-setting\n")
        (self.source / "credentials-demo.json").write_text("{}\n")
        (self.source / "appsettings.Local.json").write_text("{}\n")
        (self.source / "private-agreement.pdf").write_bytes(b"demo private document")
        # Even accidentally tracked private files are excluded.
        subprocess.run(["git", "-C", str(self.source), "add", "."], check=True)
        output = Path(self.temp.name) / "session"
        module.export_session(self.source, output)
        self.assertEqual((output / "README.md").read_text(), "Demo project\n")
        self.assertTrue((output / ".env.example").is_file())
        self.assertFalse((output / ".env").exists())
        self.assertFalse((output / "credentials-demo.json").exists())
        self.assertFalse((output / "appsettings.Local.json").exists())
        self.assertFalse((output / "private-agreement.pdf").exists())
        refs = subprocess.check_output(["git", "-C", str(output), "for-each-ref"])
        self.assertEqual(refs, b"")

    def test_refuses_existing_or_nested_output_directories(self):
        with self.assertRaises(ValueError):
            module.export_session(self.source, self.source / "session")
        with self.assertRaises(ValueError):
            module.export_session(self.source, self.source)

    def test_refuses_hardcoded_credentials_before_creating_output(self):
        (self.source / "settings.json").write_text('{"Token":"test-fixture-value-not-a-valid-credential"}')  # synthetic scanner fixture
        output = Path(self.temp.name) / "session"
        with self.assertRaises(ValueError):
            module.export_session(self.source, output)
        self.assertFalse(output.exists())

    def test_refuses_symlinks(self):
        outside = Path(self.temp.name) / "outside.txt"
        outside.write_text("private demo text")
        (self.source / "link.txt").symlink_to(outside)
        with self.assertRaises(ValueError):
            module.export_session(self.source, Path(self.temp.name) / "session")


if __name__ == "__main__":
    unittest.main()
