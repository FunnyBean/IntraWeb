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
    [Route("api/equipment")]
    public class EquipmentController : BaseController
    {
        #region Private Field

        private IEquipmentRepository _equipmentRepository;
        private ILogger<EquipmentController> _logger;
        private IMapper _mapper;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentController"/> class.
        /// </summary>
        /// <param name="equipmentRepository">The equipment repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">Mapper for mapping domain classes to model classes and reverse.</param>
        public EquipmentController(IEquipmentRepository equipmentRepository,
                           ILogger<EquipmentController> logger,
                                                 IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all equipment.
        /// </summary>
        /// <returns>All equipment</returns>
        [HttpGet]
        public IEnumerable<EquipmentViewModel> Get()
        {
            return _mapper.Map<IEnumerable<EquipmentViewModel>>(_equipmentRepository.GetAll());
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
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
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
            if (!ExistsEquipment(equipmentVm.Description))
            {
                var equipment = _mapper.Map<Equipment>(equipmentVm);

                return SaveData(() =>
                {
                    _equipmentRepository.Add(equipment);
                },
                () =>
                {
                    this.Response.StatusCode = (int) HttpStatusCode.Created;
                    return this.Json(_mapper.Map<EquipmentViewModel>(equipment));
                });
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with description '{equipmentVm.Description}' already exists." });
            }
        }

        private bool ExistsEquipment(string description)
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
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var message = $"Invalid argument. Id '{equipmentId}' and equipmentVm.Id '{equipmentVm.Id}' are not equal.";
                _logger.LogWarning(message);

                return this.Json(new { Message = message });
            }

            var editedEquipment = _equipmentRepository.GetItem(equipmentId);
            if (editedEquipment == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (ExistsAnotherEquipmentWithName(equipmentVm.Description, equipmentId))
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with name '{equipmentVm.Description}' already exists." });
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
                this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving equipment throw Exception '{ex.Message}'" });
            }
        }

        private bool ExistsAnotherEquipmentWithName(string equipmentDescription, int equipmentId)
        {
            var equipment = _equipmentRepository.GetItem(p => p.Description.Equals(equipmentDescription, StringComparison.CurrentCultureIgnoreCase));

            return equipment != null && equipment.Id != equipmentId;
        }
    }
}
