using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Models
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the Equipment model.
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// Gets all equipments.
        /// </summary>
        /// <returns>Queryalble for obtain all equipments.</returns>
        IQueryable<Equipment> GetAllEquipments();

        /// <summary>
        /// Gets the equipment.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns>
        /// Return equipment with specific id; otherwise null.
        /// </returns>
        Equipment GetEquipment(int equipmentId);      

        /// <summary>
        /// Adds the equipment.
        /// </summary>
        /// <param name="equipment">The new equipment.</param>
        void AddEquipment(Equipment equipment);

        /// <summary>
        /// Edits the specified equipment.
        /// </summary>
        /// <param name="equipment">The equipment.</param>
        void Edit(Equipment equipment);

        /// <summary>
        /// Deletes the specified equipment.
        /// </summary>
        /// <param name="equipment">The equipment for deleting.</param>
        void Delete(Equipment equipment);

        /// <summary>
        /// Deletes the specified equipment by id.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        void Delete(int equipmentId);

        /// <summary>
        /// Save changes.
        /// </summary>
        void Save();
    }
}
