import { useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { constants } from "../../commons/constants";
import { authService } from "../../services/api/authService";
import { localStorageService } from "../../services/app/localStorageService";
import { GoogleLogin } from "react-google-login";
import "./login.css";

const clientId = process.env.REACT_APP_GOOGLE_CLIENT_ID;

const Login = () => {
  const history = useHistory();

  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState("");
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState("");

  const [isLoading, setIsLoading] = useState(false);

  const resetErrors = () => {
    setEmailError("");
    setPasswordError("");
  };

  const validateInputs = () => {
    let valid = true;
    resetErrors();
    if (!email) {
      setEmailError("Please enter your email");
      valid = false;
    }

    if (!password) {
      setPasswordError("Please enter your password");
      valid = false;
    }

    return valid;
  };

  const login = async () => {
    if (!validateInputs()) return;

    setIsLoading(true);
    const result = await authService.login(email, password);
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
    history.push(constants.routing.HOME);
  };

  return (
    <div className="row vh-100">
      <div className="col-lg-4 login-box mx-auto my-auto">
        <div className="card px-3 pt-3">
          <div className="card-body">
            <h2 className="horizontal-center mb-2">Login</h2>
            <div className="horizontal-center mb-3">
              Please enter your credential.
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
            </div>
            <div className="login-options mt-3">
              <div>
                <Link to={constants.routing.FORGOT_PASSWORD}>
                  Forgot password?
                </Link>
              </div>
              <div>
                Need an account? <Link to={constants.routing.REGISTER}>Register</Link>
              </div>
            </div>
            <div className="d-grid my-3">
              <button className="btn btn-primary" onClick={login} disabled={isLoading}>
                <i className="fas fa-sign-in-alt me-2"></i>Login
              </button>
            </div>
            <hr />
            <GoogleLogin
              clientId={clientId}
              render={(renderProps) => (
                <div className="d-grid mb-3">
                  <button
                    className="btn btn-danger"
                    onClick={renderProps.onClick}
                    disabled={renderProps.disabled}
                  >
                    <i className="fab fa-google me-2"></i>Login with Google
                  </button>
                </div>
              )}
              cookiePolicy={"single_host_origin"}
              redirectUri={`${window.location.origin}${constants.routing.GOOGLE_LOGIN_REDIRECT}`}
              uxMode="redirect"
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
