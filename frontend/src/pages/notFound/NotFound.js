import { useHistory } from "react-router";
import { constants } from "../../commons/constants";

const NotFound = () => {
  const history = useHistory();
  return (
    <div className="row vh-100">
      <div className="col-lg-4 mx-auto my-auto d-flex flex-column">
        <h1 className="display-2 align-self-center">Error 404</h1>
        <h3 className="pt-3 align-self-center">Nothing there for you!</h3>
        <p className="align-self-center">
          The page you are looking for was not found.
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

export default NotFound;
