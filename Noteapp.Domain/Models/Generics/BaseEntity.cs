using Noteapp.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Domain.Models.Generics
{
    /// <summary>
    /// Base class for entities.    
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }

        public virtual object GetId()
        {
            return $"({Id})";
        }
    }

    public abstract class AuditableEntity : BaseEntity, IAuditable, ISoftDeletable
    {
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
