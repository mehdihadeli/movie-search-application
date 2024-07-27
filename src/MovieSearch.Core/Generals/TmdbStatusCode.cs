namespace MovieSearch.Core.Generals;

public enum TmdbStatusCode
{
    Unknown = 0,

    /// <summary>
    ///     200: Success.
    /// </summary>
    //[Description( "200: Success." )]
    Success = 1,

    /// <summary>
    ///     501: Invalid service: this service does not exist.
    /// </summary>
    //[Description( "501: Invalid service: this service does not exist." )]
    InvalidService = 2,

    /// <summary>
    ///     401: Authentication failed: You do not have permissions to access the service.
    /// </summary>
    //[Description( "401: Authentication failed: You do not have permissions to access the service." )]
    InsufficientPermissions = 3,

    /// <summary>
    ///     405: Invalid format: This service doesn't exist in that format.
    /// </summary>
    //[Description( "405: Invalid format: This service doesn't exist in that format." )]
    InvalidFormat = 4,

    /// <summary>
    ///     422: Invalid parameters: Your request parameters are incorrect.
    /// </summary>
    //[Description( "422: Invalid parameters: Your request parameters are incorrect." )]
    InvalidParameters = 5,

    /// <summary>
    ///     404: Invalid id: The pre-requisite id is invalid or not found.
    /// </summary>
    //[Description( "404: Invalid id: The pre-requisite id is invalid or not found." )]
    InvalidId = 6,

    /// <summary>
    ///     401: Invalid API key: You must be granted a valid key.
    /// </summary>
    //[Description( "401: Invalid API key: You must be granted a valid key." )]
    InvalidApiKey = 7,

    /// <summary>
    ///     403: Duplicate entry: The data you tried to submit already exists.
    /// </summary>
    //[Description( "403: Duplicate entry: The data you tried to submit already exists." )]
    DuplicateEntry = 8,

    /// <summary>
    ///     503: Service offline: This service is temporarily offline, try again later.
    /// </summary>
    //[Description( "503: Service offline: This service is temporarily offline, try again later." )]
    ServiceOffline = 9,

    /// <summary>
    ///     503: Service offline: This service is temporarily offline, try again later.
    /// </summary>
    //[Description( "401: Suspended API key: Access to your account has been suspended, contact TMDb." )]
    SuspendedApiKey = 10,

    /// <summary>
    ///     503: Service offline: This service is temporarily offline, try again later.
    /// </summary>
    //[Description( "500: Internal error: Something went wrong, contact TMDb." )]
    InternalError = 11,

    /// <summary>
    ///     201: The item/record was updated successfully.
    /// </summary>
    //[Description( "201: The item/record was updated successfully." )]
    SuccessfulUpdate = 12,

    /// <summary>
    ///     200: The item/record was deleted successfully.
    /// </summary>
    //[Description( "200: The item/record was deleted successfully." )]
    SuccessfulDelete = 13,

    /// <summary>
    ///     401: Authentication failed.
    /// </summary>
    //[Description( "401: Authentication failed." )]
    AuthenticationFailed = 14,

    /// <summary>
    ///     500: Failed.
    /// </summary>
    //[Description( "500: Failed." )]
    Failed = 15,

    /// <summary>
    ///     401: Device denied.
    /// </summary>
    //[Description( "401: Device denied." )]
    DeviceDenied = 16,

    /// <summary>
    ///     401: Session denied.
    /// </summary>
    //[Description( "401: Session denied." )]
    SessionDenied = 17,

    /// <summary>
    ///     400: Validation failed.
    /// </summary>
    //[Description( "400: Validation failed." )]
    ValidationFailed = 18,

    /// <summary>
    ///     406: Invalid accept header.
    /// </summary>
    //[Description( "406: Invalid accept header." )]
    InvalidAcceptHeader = 19,

    /// <summary>
    ///     422: Invalid date range: Should be a range no longer than 14 days.
    /// </summary>
    //[Description( "422: Invalid date range: Should be a range no longer than 14 days." )]
    InvalidDateRange = 20,

    /// <summary>
    ///     200: Entry not found: The item you are trying to edit cannot be found.
    /// </summary>
    //[Description( "200: Entry not found: The item you are trying to edit cannot be found." )]
    EntryNotFound = 21,

    /// <summary>
    ///     400: Invalid page: Pages start at 1 and max at 1000. They are expected to be an integer.
    /// </summary>
    //[Description( "400: Invalid page: Pages start at 1 and max at 1000. They are expected to be an integer." )]
    InvalidPage = 22,

    /// <summary>
    ///     400: Invalid date: Format needs to be YYYY-MM-DD.
    /// </summary>
    //[Description( "400: Invalid date: Format needs to be YYYY-MM-DD." )]
    InvalidDate = 23,

    /// <summary>
    ///     400: Invalid date: Format needs to be YYYY-MM-DD.
    /// </summary>
    //[Description( "504: Your request to the backend server timed out. Try again." )]
    ServerTimeout = 24,

    /// <summary>
    ///     400: Invalid date: Format needs to be YYYY-MM-DD.
    /// </summary>
    //[Description( "429: Your request count (#) is over the allowed limit of (40)." )]
    RequestOverLimit = 25,

    /// <summary>
    ///     "400: You must provide a username and password.
    /// </summary>
    //[Description( "400: You must provide a username and password." )]
    AuthenticationRequired = 26,

    /// <summary>
    ///     400: Too many append to response objects: The maximum number of remote calls is 20.
    /// </summary>
    //[Description( "400: Too many append to response objects: The maximum number of remote calls is 20." )]
    ResponseObjectOverflow = 27,

    /// <summary>
    ///     400: Invalid timezone: Please consult the documentation for a valid timezone.
    /// </summary>
    //[Description( "400: Invalid timezone: Please consult the documentation for a valid timezone." )]
    InvalidTimezone = 28,

    /// <summary>
    ///     400: Invalid timezone: Please consult the documentation for a valid timezone.
    /// </summary>
    //[Description( "400: You must confirm this action: Please provide a confirm=true parameter." )]
    ActionMustBeConfirmed = 29,

    /// <summary>
    ///     401: Invalid username and/or password: You did not provide a valid login.
    /// </summary>
    //[Description( "401: Invalid username and/or password: You did not provide a valid login." )]
    InvalidAuthentication = 30,

    /// <summary>
    ///     401: Account disabled: Your account is no longer active. Contact TMDb if this is an error.
    /// </summary>
    //[Description( "401: Account disabled: Your account is no longer active. Contact TMDb if this is an error." )]
    AccountDisabled = 31,

    /// <summary>
    ///     401: Email not verified: Your email address has not been verified.
    /// </summary>
    //[Description( "401: Email not verified: Your email address has not been verified." )]
    EmailNotVerified = 32,

    /// <summary>
    ///     401: Invalid request token: The request token is either expired or invalid.
    /// </summary>
    //[Description( "401: Invalid request token: The request token is either expired or invalid." )]
    InvalidRequestToken = 33,

    /// <summary>
    ///     401: The resource you requested could not be found.
    /// </summary>
    //[Description( "401: The resource you requested could not be found." )]
    ResourceNotFound = 34
}
