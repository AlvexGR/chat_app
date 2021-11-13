import { useEffect, useState } from "react";
import { useHistory, useParams } from "react-router";
import { constants } from "../../commons/constants";
import { userService } from "../../services/api/userService";
import { localStorageService } from "../../services/app/localStorageService";

const AccountConfirmation = () => {
  const { token } = useParams();
  const history = useHistory();
  const status = {
    PENDING: 0,
    IN_PROGRESS: 1,
    SUCCESS: 2,
    ERROR: 3,
  };
  const [confirmStatus, setConfirmStatus] = useState(status.PENDING);

  useEffect(() => {
    console.log(token);
    if (!token) return history.push(constants.routing.LOGIN);
    confirm();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [token]);

  const confirm = async () => {
    setConfirmStatus(status.IN_PROGRESS);
    const result = await userService.confirmAccount(token);
    if (!result || !result.success) {
      setConfirmStatus(status.ERROR);
      return;
    }
    setConfirmStatus(status.SUCCESS);

    localStorageService.setValue(
      constants.storeKeys.LOGIN_USER,
      result.data.user
    );
    localStorageService.setValue(
      constants.storeKeys.ACCESS_TOKEN,
      result.data.token
    );
  };

  const Status = () => {
    switch (confirmStatus) {
      case status.PENDING:
      case status.IN_PROGRESS: {
        return (
          <div>
            <img
              alt="confirm-in-progress"
              className="rounded mx-auto d-block"
              src="/images/in_progress.png"
            />
            <div className="horizontal-center fz-17px">
              Confirming your account. Please wait
            </div>
          </div>
        );
      }
      case status.SUCCESS: {
        return (
          <div>
            <img
              alt="confirm-success"
              className="rounded mx-auto d-block"
              src="/images/celebrating.png"
            />
            <div className="mt-3 fz-17px horizontal-center">
              Welcome to my ChatApp, your account is confirmed. It's time to
              connect and chat with your friends
            </div>
            <div className="horizontal-center mt-2">
              <button className="btn btn-primary px-4">Continue</button>
            </div>
          </div>
        );
      }
      case status.ERROR: {
        return (
          <div>
            <img
              alt="confirm-error"
              className="rounded mx-auto d-block"
              src="/images/error.png"
            />
            <p className="align-center mt-3 fz-17px">
              Something went wrong. Please try again later
            </p>
            <div className="horizontal-center mt-2">
              <button className="btn btn-warning px-4" onClick={confirm}>
                Retry
              </button>
            </div>
          </div>
        );
      }
      default:
        break;
    }
  };

  return (
    <div className="row vh-100">
      <div className="col-lg-6 mx-auto my-auto">
        <div className="card">
          <div className="card-body">
            <h4 className="align-center">Account confirmation</h4>
            <Status />
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountConfirmation;
