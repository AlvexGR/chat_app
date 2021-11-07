import { useHistory } from "react-router";
import { constants } from "../../commons/constants";

const Forbidden = () => {
  const history = useHistory();
  return (
    <div className="row vh-100">
      <div className="col-lg-4 my-auto mx-auto d-flex flex-column">
        <h1 className="display-2 align-self-center">Error 403</h1>
        <h3 className="pt-3 align-self-center">Stop right there!</h3>
        <p className="align-self-center">
          You don't have permission to access this page.
        </p>
        <div className="align-self-center">
          <button
            className="btn btn-primary"
            onClick={() => {
              history.push(constants.routing.HOME);
            }}
          >
            Back to home
          </button>
        </div>
      </div>
    </div>
  );
};

export default Forbidden;
