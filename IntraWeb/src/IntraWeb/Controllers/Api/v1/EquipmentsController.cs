using System;
using System.Collections.Generic;
using System.Net;
using IntraWeb.Filters;
using IntraWeb.Models.Rooms;
using IntraWeb.ViewModels.Rooms;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace IntraWeb.Controllers.Api.v1
{
    [Route("api/equipments")]
    public class EquipmentsController : BaseController
    {
        #region Private Field

        private IEquipmentRepository _equipmentRepository;
        private ILogger<EquipmentsController> _logger;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentsController"/> class.
        /// </summary>
        /// <param name="equipmentRepository">The equipment repository.</param>
        /// <param name="logger">The logger.</param>
        public EquipmentsController(IEquipmentRepository equipmentRepository,
                           ILogger<EquipmentsController> logger)
        {
            _equipmentRepository = equipmentRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all equipments.
        /// </summary>
        /// <returns>All equipments</returns>
        [HttpGet]
        public IEnumerable<EquipmentViewModel> Get()
        {
            var equipments = AutoMapper.Mapper.Map<IEnumerable<EquipmentViewModel>>(_equipmentRepository.GetAll());

            return equipments;
        }

        /// <summary>
        /// Gets equipment by Id.
        /// </summary>
        /// <param name="equipmentId">Equipment Id.</param>
        /// <returns>Equipment with specific Id. Null if doesn't exist.</returns>
        [HttpGet("{equipmentId}", Name = "GetEquipment")]
        public IActionResult Get(int equipmentId)
        {
            var equipment = _equipmentRepository.GetItem(equipmentId);

            if (equipment == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NoContent;
                return this.Json(null);
            }
            else
            {
                return this.Json(AutoMapper.Mapper.Map<EquipmentViewModel>(equipment));
            }
        }

        /// <summary>
        /// Post new equipment.
        /// </summary>
        /// <param name="equipmentVm">New equipment.</param>
        /// <returns>Added equipment.</returns>
        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokia¾ sa nespraví autorizácia
        public IActionResult Post([FromBody] EquipmentViewModel equipmentVm)
        {
            if (!ExistEquipment(equipmentVm.Description))
            {
                var equipment = AutoMapper.Mapper.Map<Equipment>(equipmentVm);

                return SaveData(() =>
                {
                    _equipmentRepository.Add(equipment);
                },
                () =>
                {
                    this.Response.StatusCode = (int) HttpStatusCode.Created;
                    return this.Json(AutoMapper.Mapper.Map<EquipmentViewModel>(equipment));
                });
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with description '{equipmentVm.Description}' already exist." });
            }
        }

        private bool ExistEquipment(string description)
        {
            return _equipmentRepository.GetItem(p => p.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        //Put

        //Delete

        private IActionResult SaveData(Action beforeAction,
                          Func<IActionResult> result)
        {
            try
            {
                beforeAction();
                _equipmentRepository.Save();

                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data in EquipmentController.", ex);
                this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving equipment throw Exception '{ex.Message}'" });
            }
        }
    }
}
