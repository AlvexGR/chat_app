import { useState } from "react";
import { useDispatch } from "react-redux";
import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import { constants } from "../../commons/constants";
import { messages } from "../../commons/messages";
import { storeActions } from "../../commons/storeActions";
import { authService } from "../../services/api/authService";
import "./forgotPassword.css";

const ForgotPassword = () => {
  const history = useHistory();
  const dispatch = useDispatch();
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState("");

  const [isLoading, setIsLoading] = useState(false);

  const resetErrors = () => {
    setEmailError("");
  };

  const validateInputs = () => {
    let valid = true;
    resetErrors();
    if (!email) {
      setEmailError("Please enter your email");
      valid = false;
    }

    return valid;
  };

  const forgotPassword = async () => {
    if (!validateInputs()) return;

    setIsLoading(true);
    const result = await authService.forgotPassword(email);
    setIsLoading(false);

    if (!result || !result.success) return;

    dispatch({
      type: storeActions.toaster.set,
      ...{
        toasterType: messages.toasterTypes.SUCCESS,
        message: messages.successMessages.FORGOT_PASSWORD,
      },
    });

    history.push(constants.routing.LOGIN);
  };

  return (
    <div className="row vh-100">
      <div className="col-lg-6 fw-box mx-auto my-auto">
        <div className="card px-3 py-3">
          <div className="card-body">
            <h2 className="horizontal-center mb-2">Forgot password</h2>
            <div className="horizontal-center mb-3">
              Please enter your email.
            </div>
            <div className="form-group">
              <label>Email:</label>
              <div className="input-group">
                <span className="input-group-text">
                  <i className="fas fa-envelope"></i>
                </span>
                <input
                  className="form-control"
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>
              <div hidden={!emailError} className="text-danger">
                {emailError}
              </div>
            </div>
            <div className="horizontal-center mt-3">
              <p>
                Remember now?{" "}
                <Link to={constants.routing.LOGIN}>Login</Link>
              </p>
            </div>
            <div className="d-grid">
              <button disabled={isLoading} className="btn btn-primary" onClick={forgotPassword}>
                <i className="fas fa-paper-plane me-2"></i>Submit
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ForgotPassword;
