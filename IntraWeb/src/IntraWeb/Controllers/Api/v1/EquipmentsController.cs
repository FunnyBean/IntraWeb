using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
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
        private IMapper _mapper;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentsController"/> class.
        /// </summary>
        /// <param name="equipmentRepository">The equipment repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">Mapper for mapping domain classes to model classes and reverse.</param>
        public EquipmentsController(IEquipmentRepository equipmentRepository,
                           ILogger<EquipmentsController> logger,
                                                 IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all equipments.
        /// </summary>
        /// <returns>All equipments</returns>
        [HttpGet]
        public IEnumerable<EquipmentViewModel> Get()
        {
            var equipments = _mapper.Map<IEnumerable<EquipmentViewModel>>(_equipmentRepository.GetAll());

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
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<EquipmentViewModel>(equipment));
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
                var equipment = _mapper.Map<Equipment>(equipmentVm);

                return SaveData(() =>
                {
                    _equipmentRepository.Add(equipment);
                },
                () =>
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Created;
                    return this.Json(_mapper.Map<EquipmentViewModel>(equipment));
                });
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with description '{equipmentVm.Description}' already exist." });
            }
        }

        private bool ExistEquipment(string description)
        {
            return _equipmentRepository.GetItem(p => p.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        /// <summary>
        /// Update the equipment.
        /// </summary>
        /// <param name="equipmentId">Equipment id for update.</param>
        /// <param name="equipmentVm">Equipment view model, with new properties.</param>
        [HttpPut("{equipmentId}")]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Put(int equipmentId, [FromBody] EquipmentViewModel equipmentVm)
        {
            if (equipmentVm.Id != equipmentId)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = $"Invalid argument. Id '{equipmentId}' and equipmentVm.Id '{equipmentVm.Id}' are not equal.";
                _logger.LogWarning(message);

                return this.Json(new { Message = message });
            }

            var editedEquipment = _equipmentRepository.GetItem(equipmentId);
            if (editedEquipment == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (ExistAnotherEquipmentWithName(equipmentVm.Description, equipmentId))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with name '{equipmentVm.Description}' already exist." });
            }
            else
            {
                editedEquipment = _mapper.Map(equipmentVm, editedEquipment);

                return SaveData(() =>
                {
                    _equipmentRepository.Edit(editedEquipment);
                });
            }
        }

        /// <summary>
        /// Deletes the specified equipment.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        [HttpDelete("{equipmentId}")]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokia¾ sa nespraví autorizácia
        public IActionResult Delete(int equipmentId)
        {
            return SaveData(() =>
            {
                _equipmentRepository.Delete(equipmentId);
            });
        }

        private IActionResult SaveData(Action beforeAction)
        {
            return SaveData(beforeAction, () => this.Json(null));
        }

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
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving equipment throw Exception '{ex.Message}'" });
            }
        }

        private bool ExistAnotherEquipmentWithName(string equipmentDescription, int equipmentId)
        {
            var equipment = _equipmentRepository.GetItem(p => p.Description.Equals(equipmentDescription, StringComparison.CurrentCultureIgnoreCase));

            return equipment != null && equipment.Id != equipmentId;
        }
    }
}