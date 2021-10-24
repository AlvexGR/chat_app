// @flow
import { useEffect } from "react";
import { connect } from "react-redux";
import { Flip, toast, ToastContainer } from "react-toastify";
import { messages } from "../../commons/messages";

const Toaster = (props) => {
  useEffect(() => {
    if (!props.toasterReducer.counter) return;
    switch (props.toasterReducer.toasterType) {
      case messages.toasterTypes.SUCCESS:
        toast.success(props.toasterReducer.message);
        break;
      case messages.toasterTypes.ERROR:
        toast.error(props.toasterReducer.message);
        break;
      case messages.toasterTypes.WARNING:
        toast.warning(props.toasterReducer.message);
        break;
      default:
        break;
    }
  }, [props.toasterReducer.counter]);

  return (
    <ToastContainer
      position={toast.POSITION.BOTTOM_CENTER}
      transition={Flip}
      autoClose={2000}
      limit={3}
      hideProgressBar={false}
      newestOnTop={false}
      closeOnClick
      rtl={false}
      pauseOnFocusLoss
      draggable
      pauseOnHover
    />
  );
};

const mapStateToProps = (state) => {
  return { toasterReducer: state.toasterReducer };
};

export default connect(mapStateToProps)(Toaster);
