using Krosbook.Services.Template;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Data for email message for resetting password.
    /// </summary>
    public class RoomReservation: BaseEmailData
    {

        #region Constructors

        /// <summary>
        /// Initializes data with link for password reset <paramref name="title" />.
        /// </summary>
        /// <param name="title">Link where user can reset his password.</param>
        public RoomReservation(string title) : base("ReservationRoom")
        {
            this.title = title;
        }

        #endregion


        #region General

        /// <summary>
        /// Link where user can reset his password.
        /// </summary>
        [TemplateVariable("title")]
        public string title { get; }

        #endregion

    }
}
