import { useEffect } from "react";
import { connect, useDispatch } from "react-redux";
import { useHistory } from "react-router";
import { constants } from "../../commons/constants";
import { storeActions } from "../../commons/storeActions";
import { authService } from "../../services/api/authService";

const Auth = (props) => {
  const history = useHistory();
  const dispatch = useDispatch();
  useEffect(() => {
    if (authService.isLoggedIn()) {
      dispatch({
        type: storeActions.auth.set,
        skipCounter: true,
        ...{
          authorized: true,
          isAdmin: authService.isAdmin()
        },
      });
      return;
    }
    if (!props.authReducer.authorized) {
      history.push(constants.routing.LOGIN);
      return;
    }
  }, [props.authReducer.counter]);

  return props.component;
};

const mapStateToProps = (state) => {
  return { authReducer: state.authReducer };
};

export default connect(mapStateToProps)(Auth);
