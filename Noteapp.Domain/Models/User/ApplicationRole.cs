using Microsoft.AspNetCore.Identity;
using Noteapp.Domain.Models.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Domain.Models.User
{
    public class ApplicationRole : IdentityRole<Guid>, IAuditable, ISoftDeletable
    {
        /// <summary>
        /// Collection of users containing this role
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }

        #region Implementation of IAuditable

        /// <summary>
        /// Gets or sets the UTC date time when the entity was first created.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC date time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user that created this entity.
        /// </summary>
        public Guid? CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that created this entity.
        /// </summary>
        public virtual ApplicationUser? CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Id of the user that last updated this entity.
        /// </summary>
        public Guid? UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that last updated this entity.
        /// </summary>
        public virtual ApplicationUser? UpdatedBy { get; set; }

        /// <summary>
        /// Shows if the entity is active or not
        /// </summary>
        public bool IsActive { get; set; }
        #endregion

        #region Implementation of ISoftDeletable

        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion
    }
}
