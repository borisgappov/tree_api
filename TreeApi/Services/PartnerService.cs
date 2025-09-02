using AutoMapper;
using TreeApi.Data.Entities;
using TreeApi.Data.UnitOfWork;
using TreeApi.Models;

namespace TreeApi.Services
{
    public interface IPartnerService
    {
        Task<IEnumerable<MPartner>> GetAllPartnersAsync();
        Task<MPartner?> GetPartnerByIdAsync(long id);
        Task<MPartner?> GetPartnerByCodeAsync(string code);
        Task<MPartner> CreatePartnerAsync(MPartner partnerModel);
        Task<MPartner> UpdatePartnerAsync(long id, MPartner partnerModel);
        Task DeletePartnerAsync(long id);
        Task<bool> PartnerExistsAsync(long id);
        Task<bool> CodeExistsAsync(string code);
        Task<MPartner> RememberPartnerByCodeAsync(string code);
    }
    
    public class PartnerService(IUnitOfWork unitOfWork, IMapper mapper) : IPartnerService
    {
        
        /// <summary>
        /// Retrieves all partners from the database
        /// </summary>
        /// <returns>Collection of partner models mapped from entities</returns>
        public async Task<IEnumerable<MPartner>> GetAllPartnersAsync()
        {
            var partners = await unitOfWork.Partners.GetAllAsync();
            return partners.Select(partner => mapper.Map<MPartner>(partner));
        }
        
        /// <summary>
        /// Retrieves a specific partner by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the partner</param>
        /// <returns>Partner model if found, null otherwise</returns>
        public async Task<MPartner?> GetPartnerByIdAsync(long id)
        {
            var partner = await unitOfWork.Partners.GetByIdAsync(id);
            return partner != null ? mapper.Map<MPartner>(partner) : null;
        }
        
        /// <summary>
        /// Retrieves a specific partner by its unique code
        /// </summary>
        /// <param name="code">The unique code of the partner</param>
        /// <returns>Partner model if found, null otherwise</returns>
        public async Task<MPartner?> GetPartnerByCodeAsync(string code)
        {
            var partner = await unitOfWork.Partners.GetByCodeAsync(code);
            return partner != null ? mapper.Map<MPartner>(partner) : null;
        }
        
        /// <summary>
        /// Creates a new partner in the database
        /// </summary>
        /// <param name="partnerModel">The partner model containing the partner data</param>
        /// <returns>The created partner model with generated ID</returns>
        /// <exception cref="InvalidOperationException">Thrown when partner with specified code already exists</exception>
        public async Task<MPartner> CreatePartnerAsync(MPartner partnerModel)
        {
            if (await unitOfWork.Partners.CodeExistsAsync(partnerModel.Code))
                throw new InvalidOperationException($"Partner with code '{partnerModel.Code}' already exists");
            
            var partner = mapper.Map<Partner>(partnerModel);
            await unitOfWork.Partners.AddAsync(partner);
            await unitOfWork.SaveChangesAsync();
            
            return mapper.Map<MPartner>(partner);
        }
        
        /// <summary>
        /// Updates an existing partner's information
        /// </summary>
        /// <param name="id">The unique identifier of the partner to update</param>
        /// <param name="partnerModel">The updated partner data</param>
        /// <returns>The updated partner model</returns>
        /// <exception cref="ArgumentException">Thrown when partner with specified ID is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when new code conflicts with existing partner</exception>
        public async Task<MPartner> UpdatePartnerAsync(long id, MPartner partnerModel)
        {
            var existingPartner = await unitOfWork.Partners.GetByIdAsync(id);
            if (existingPartner == null)
                throw new ArgumentException($"Partner with id {id} not found");
            
            if (existingPartner.Code != partnerModel.Code && 
                await unitOfWork.Partners.CodeExistsAsync(partnerModel.Code))
                throw new InvalidOperationException($"Partner with code '{partnerModel.Code}' already exists");
            
            existingPartner.Code = partnerModel.Code;
            existingPartner.UpdatedAt = DateTime.UtcNow;
            
            await unitOfWork.Partners.UpdateAsync(existingPartner);
            await unitOfWork.SaveChangesAsync();
            
            return mapper.Map<MPartner>(existingPartner);
        }
        
        /// <summary>
        /// Deletes a partner from the database
        /// </summary>
        /// <param name="id">The unique identifier of the partner to delete</param>
        /// <exception cref="ArgumentException">Thrown when partner with specified ID is not found</exception>
        public async Task DeletePartnerAsync(long id)
        {
            var partner = await unitOfWork.Partners.GetByIdAsync(id);
            if (partner == null)
                throw new ArgumentException($"Partner with id {id} not found");
            
            await unitOfWork.Partners.DeleteAsync(partner);
            await unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Checks if a partner exists by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the partner</param>
        /// <returns>True if partner exists, false otherwise</returns>
        public async Task<bool> PartnerExistsAsync(long id)
        {
            return await unitOfWork.Partners.ExistsAsync(id);
        }
        
        /// <summary>
        /// Checks if a partner exists by its unique code
        /// </summary>
        /// <param name="code">The unique code of the partner</param>
        /// <returns>True if partner exists, false otherwise</returns>
        public async Task<bool> CodeExistsAsync(string code)
        {
            return await unitOfWork.Partners.CodeExistsAsync(code);
        }
        
        /// <summary>
        /// Creates a new partner with the specified code if it doesn't exist
        /// </summary>
        /// <param name="code">The unique code for the partner</param>
        /// <returns>The created partner model with generated ID</returns>
        /// <exception cref="InvalidOperationException">Thrown when partner with specified code already exists</exception>
        public async Task<MPartner> RememberPartnerByCodeAsync(string code)
        {
            if (await unitOfWork.Partners.CodeExistsAsync(code))
                throw new InvalidOperationException($"Partner with code '{code}' already exists");
            
            var partner = new Partner
            {
                Code = code,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await unitOfWork.Partners.AddAsync(partner);
            await unitOfWork.SaveChangesAsync();
            
            return mapper.Map<MPartner>(partner);
        }
    }
}
