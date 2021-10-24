import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import Button from "react-bootstrap/Button";
import { useHistory } from "react-router";
import { constants } from "../../commons/constants";

const Forbidden = () => {
  const history = useHistory();
  return (
    <Container>
      <Row className="align-center vh-100">
        <Col md="4" className="d-flex flex-column">
          <h1 className="display-2 align-self-center">403!</h1>
          <h3 className="pt-3 align-self-center">Stop right there</h3>
          <p className="align-self-center">
            You don't have permission to access this page.
          </p>
          <Button
            onClick={() => {
              history.push(constants.routing.HOME);
            }}
          >
            Back to home
          </Button>
        </Col>
      </Row>
    </Container>
  );
};

export default Forbidden;
