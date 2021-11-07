const toasterTypes = {
  SUCCESS: "SUCCESS",
  ERROR: "ERROR",
  WARNING: "WARNING",
}

const errorCodes = {
  INTERNAL_SERVER_ERROR: "INTERNAL_SERVER_ERROR",
  BAD_REQUEST: "BAD_REQUEST",
  NOT_FOUND: "NOT_FOUND",
  EMAIL_EXISTS: "EMAIL_EXISTS",
  INVALID_CREDENTIAL: "INVALID_CREDENTIAL",
  FORBIDDEN: "FORBIDDEN",
  SAME_NEW_PASSWORD: "SAME_NEW_PASSWORD",
  INCORRECT_CURRENT_PASSWORD: "INCORRECT_CURRENT_PASSWORD"
}

const errorMessages = {
  INTERNAL_SERVER_ERROR: "Something went wrong! Please try again later",
  BAD_REQUEST: "Something went wrong! Please try again later",
  NOT_FOUND: "Data not long exists",
  EMAIL_EXISTS: "This email is taken",
  INVALID_CREDENTIAL: "Invalid credential. Please try again",
  FORBIDDEN: "You don't have permission to access this page",
  SAME_NEW_PASSWORD: "New password must be different from current password",
  INCORRECT_CURRENT_PASSWORD: "Current password is incorrect"
}

export const messages = {
  toasterTypes,
  errorCodes,
  errorMessages
}
