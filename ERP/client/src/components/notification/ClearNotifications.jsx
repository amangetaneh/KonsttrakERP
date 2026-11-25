import PropTypes from "prop-types";
import { Button } from "react-bootstrap";
import { useMutation, useQueryClient } from "react-query";
import { clearAllNotifications } from "../../api/notification";

function ClearNotifications({ notificationIds }) {
  const queryClient = useQueryClient();

  const { mutate } = useMutation(clearAllNotifications, {
    onSuccess: () => queryClient.invalidateQueries(["notifications"]),
  });

  function handleClear(e) {
    if (notificationIds?.length > 0) mutate({ notificationIds });
  }

  return (
    <div className="d-grid">
      <Button className="btn-teal border-0 m-2" onClick={handleClear}>
        Clear All Notifications
      </Button>
    </div>
  );
}

ClearNotifications.propTypes = {
  notificationIds: PropTypes.array.isRequired,
};

export default ClearNotifications;
