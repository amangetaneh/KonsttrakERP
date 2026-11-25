import importlib.util
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
spec = importlib.util.spec_from_file_location("secret_check", ROOT / "scripts/check-secrets.py")
module = importlib.util.module_from_spec(spec)
spec.loader.exec_module(module)


class CredentialScanTests(unittest.TestCase):
    def test_connection_string_with_shell_variable_is_not_a_literal_credential(self):
        data = b'CONNECTION="Server=localhost;Password=$ERP_TEST_PASSWORD;Database=demo"'  # synthetic scanner fixture
        self.assertEqual(module.findings("scripts/demo.sh", data), [])

    def test_literal_connection_password_is_reported_without_returning_the_value(self):
        data = b'CONNECTION="Server=localhost;Password=fixture-only-not-a-real-password;Database=demo"'  # synthetic scanner fixture
        findings = module.findings("scripts/demo.sh", data)
        self.assertTrue(findings)
        self.assertTrue(all(len(row) == 3 and row[0] == "scripts/demo.sh" for row in findings))

    def test_test_files_are_not_blanket_exempt_from_credential_scans(self):
        data = b'{"Token":"opaque-fixture-only-not-a-real-credential"}'  # synthetic scanner fixture
        self.assertTrue(module.findings("tests/example.py", data))

    def test_a_dollar_string_is_still_a_literal_in_json(self):
        data = b'{"Connection":"Server=localhost;Password=$ERP_TEST_PASSWORD;Database=demo"}'  # synthetic scanner fixture
        self.assertTrue(module.findings("settings.json", data))


if __name__ == "__main__":
    unittest.main()
