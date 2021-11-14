import { useState } from "react";
import { useDispatch } from "react-redux";
import { Link, useHistory } from "react-router-dom";
import { constants } from "../../commons/constants";
import { messages } from "../../commons/messages";
import { storeActions } from "../../commons/storeActions";
import { authService } from "../../services/api/authService";
import { localStorageService } from "../../services/app/localStorageService";
import "./register.css";

const Register = () => {
  const dispatch = useDispatch();
  const history = useHistory();

  const [firstName, setFirstName] = useState("");
  const [firstNameError, setFirstNameError] = useState("");
  const [lastName, setLastName] = useState("");
  const [lastNameError, setLastNameError] = useState("");
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState("");
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [confirmPasswordError, setConfirmPasswordError] = useState("");

  const [isLoading, setIsLoading] = useState(false);

  const resetErrors = () => {
    setFirstNameError("");
    setLastNameError("");
    setEmailError("");
    setPasswordError("");
    setConfirmPasswordError("");
  };

  const validateInputs = () => {
    let valid = true;
    resetErrors();

    if (!firstName) {
      setFirstNameError("Please enter your first name");
      valid = false;
    }

    if (!lastName) {
      setLastNameError("Please enter your last name");
      valid = false;
    }

    if (!email) {
      setEmailError("Please enter your email");
      valid = false;
    } else if (!constants.regexPatterns.EMAIL.test(email)) {
      setEmailError("Your email is invalid");
      valid = false;
    }

    if (!password) {
      setPasswordError("Please enter your password");
      valid = false;
    } else if (!constants.regexPatterns.PASSWORD.test(password)) {
      setPasswordError("Your password is not strong enough");
      valid = false;
    }

    if (!confirmPassword) {
      setConfirmPasswordError("Please enter your confirm password");
      valid = false;
    } else if (password !== confirmPassword) {
      setConfirmPasswordError("Both passwords must be matched");
      valid = false;
    }

    return valid;
  };
  const register = async () => {
    if (!validateInputs()) return;

    setIsLoading(true);
    const result = await authService.register({
      firstName,
      lastName,
      email,
      password,
    });
    setIsLoading(false);

    if (!result || !result.success) return;
    localStorageService.setValue(
      constants.storeKeys.LOGIN_USER,
      JSON.stringify(result.data.user)
    );
    localStorageService.setValue(
      constants.storeKeys.ACCESS_TOKEN,
      result.data.token
    );

    dispatch({
      type: storeActions.toaster.set,
      ...{
        toasterType: messages.toasterTypes.SUCCESS,
        message: messages.successMessages.REGISTER,
      },
    });

    history.push(constants.routing.HOME);
  };

  return (
    <div className="row vh-100">
      <div className="col-lg-4 register-box mx-auto my-auto">
        <div className="card px-3 pt-3">
          <div className="card-body">
            <h2 className="horizontal-center mb-2">Register</h2>
            <div className="horizontal-center mb-3">
              Please enter your account information.
            </div>
            <div className="row">
              <div className="col-md-6 form-group">
                <label>First name:</label>
                <input
                  className="form-control"
                  onChange={(e) => setFirstName(e.target.value)}
                />
                <div hidden={!firstNameError} className="text-danger">
                  {firstNameError}
                </div>
              </div>
              <div className="col-md-6 form-group last-name-ctrl">
                <label>Last name:</label>
                <input
                  className="form-control"
                  onChange={(e) => setLastName(e.target.value)}
                />
                <div hidden={!lastNameError} className="text-danger">
                  {lastNameError}
                </div>
              </div>
            </div>
            <div className="form-group mt-3">
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
            <div className="form-group mt-3">
              <label>Password:</label>
              <div className="input-group">
                <span className="input-group-text">
                  <i className="fas fa-lock"></i>
                </span>
                <input
                  type="password"
                  className="form-control"
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>
              <div hidden={!passwordError} className="text-danger">
                {passwordError}
              </div>
              <div>
                <small>
                  Password requirements:
                  <ul>
                    <li>Contains one lower case letter</li>
                    <li>Contains one UPPER CASE letter</li>
                    <li>Contains 1 digit</li>
                    <li>Contains _one spec!al ch@racter</li>
                    <li>Minimum length is 7</li>
                  </ul>
                </small>
              </div>
            </div>
            <div className="form-group mt-3">
              <label>Confirm password:</label>
              <div className="input-group">
                <span className="input-group-text">
                  <i className="fas fa-lock"></i>
                </span>
                <input
                  type="password"
                  className="form-control"
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
              </div>
              <div hidden={!confirmPasswordError} className="text-danger">
                {confirmPasswordError}
              </div>
            </div>
            <div className="horizontal-center mt-3">
              <p>
                Already have an account?{" "}
                <Link to={constants.routing.LOGIN}>Login</Link>
              </p>
            </div>
            <div className="d-grid mb-3">
              <button
                className="btn btn-success"
                onClick={register}
                disabled={isLoading}
              >
                <i className="fas fa-user-plus me-2"></i>Register
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;
