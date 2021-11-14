import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import { constants } from "./commons/constants";
import Auth from "./components/auth/Auth";
import Toaster from "./components/toaster/Toaster";

const loading = (
  <div className="pt-3 text-center">
    <div className="sk-spinner sk-spinner-pulse"></div>
  </div>
);

const Login = React.lazy(() => import("./pages/login/Login"));
const GoogleLogin = React.lazy(() => import("./pages/login/GoogleLogin"));
const Register = React.lazy(() => import("./pages/register/Register"));
const AccountConfirmation = React.lazy(() =>
  import("./pages/accountConfirmation/AccountConfirmation")
);
const ForgotPassword = React.lazy(() =>
  import("./pages/forgotPassword/ForgotPassword")
);
const Forbidden = React.lazy(() => import("./pages/forbidden/Forbidden"));
const NotFound = React.lazy(() => import("./pages/notFound/NotFound"));
const Layout = React.lazy(() => import("./layouts/Layout"));

function App() {
  return (
    <div className="container-fluid">
      <BrowserRouter>
        <React.Suspense fallback={loading}>
          <Switch>
            <Route
              exact
              path={constants.routing.LOGIN}
              name="Login"
              render={(props) => <Login {...props} />}
            />
            <Route
              exact
              path={constants.routing.GOOGLE_LOGIN_REDIRECT}
              name="Google login"
              render={(props) => <GoogleLogin {...props} />}
            />
            <Route
              exact
              path={constants.routing.REGISTER}
              name="Register"
              render={(props) => <Register {...props} />}
            />
            <Route
              exact
              path={constants.routing.ACCOUNT_CONFIRMATION}
              name="Account confirmation"
              render={(props) => <AccountConfirmation {...props} />}
            />
            <Route
              exact
              path={constants.routing.FORGOT_PASSWORD}
              name="Forgot password"
              render={(props) => <ForgotPassword {...props} />}
            />
            <Route
              exact
              path={constants.routing.FORBIDDEN}
              name="Forbidden"
              render={(props) => <Forbidden {...props} />}
            />
            <Route
              exact
              path={constants.routing.HOME}
              name="Home"
              render={(props) => <Auth component={<Layout {...props} />} />}
            />
            <Route
              path={constants.routing.NOT_FOUND}
              name="Not found"
              render={(props) => <NotFound {...props} />}
            />
          </Switch>
        </React.Suspense>
      </BrowserRouter>
      <Toaster />
    </div>
  );
}

export default App;
