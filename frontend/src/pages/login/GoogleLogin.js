import { useEffect, useState } from "react";
import { useHistory, useLocation } from "react-router";
import { constants } from "../../commons/constants";
import { authService } from "../../services/api/authService";
import { localStorageService } from "../../services/app/localStorageService";

const GoogleLogin = () => {
  const history = useHistory();
  const location = useLocation();
  const query = new URLSearchParams(
    `?${location.hash.substr(1, location.hash.length - 1)}`
  );
  const [googleToken] = useState(query.get("id_token"));

  useEffect(() => {
    if (!googleToken) history.push(constants.routing.LOGIN);
    googleLogin(googleToken);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [googleToken]);

  const googleLogin = async (token) => {
    const result = await authService.googleLogin(token);
    if (!result || !result.success) {
      history.push(constants.routing.LOGIN);
      return;
    }
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
      <div className="col-lg-6 horizontal-center mx-auto my-auto">
        <h3>Redirecting... Please wait!</h3>
      </div>
    </div>
  );
};

export default GoogleLogin;
