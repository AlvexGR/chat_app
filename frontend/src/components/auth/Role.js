import { useEffect } from "react";
import { connect, useDispatch } from "react-redux";
import { useHistory } from "react-router";
import { constants } from "../../commons/constants";
import { storeActions } from "../../commons/storeActions";
import { authService } from "../../services/api/authService";

const Role = (props) => {
  const history = useHistory();
  const dispatch = useDispatch();
  useEffect(() => {
    if (!props.admin) return;
    if (!authService.isAdmin()) {
      history.push(constants.routing.FORBIDDEN);
      return;
    }
    if (!props.authReducer.allowAccess) {
      dispatch({
        type: storeActions.auth.set,
        skipCounter: true,
        ...{
          allowAccess: true,
          isAdmin: authService.isAdmin(),
        },
      });
      history.push(constants.routing.FORBIDDEN);
      return;
    }
  }, [props.authReducer.counter]);

  return props.component;
};

const mapStateToProps = (state) => {
  return { authReducer: state.authReducer };
};

export default connect(mapStateToProps)(Role);
