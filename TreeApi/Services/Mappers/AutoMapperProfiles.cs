using AutoMapper;
using TreeApi.Data.Entities;
using TreeApi.Models;

namespace TreeApi.Services.Mappers
{
    public class TreeProfile : Profile
    {
        public TreeProfile()
        {
            CreateMap<Tree, MNode>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Nodes.Where(n => n.ParentId == null)));
                
            CreateMap<MNode, Tree>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Nodes, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
    
    public class NodeProfile : Profile
    {
        public NodeProfile()
        {
            CreateMap<Node, MNode>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));
                
            CreateMap<MNode, Node>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ForMember(dest => dest.TreeId, opt => opt.Ignore())
                .ForMember(dest => dest.ParentId, opt => opt.Ignore())
                .ForMember(dest => dest.Tree, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
    
    public class ExceptionJournalProfile : Profile
    {
        public ExceptionJournalProfile()
        {
            CreateMap<ExceptionJournal, MJournal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ExceptionMessage ?? src.StackTrace));
                
            CreateMap<ExceptionJournal, MJournalInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Timestamp));
                
            CreateMap<MJournal, ExceptionJournal>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ExceptionMessage, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.StackTrace, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.QueryParameters, opt => opt.Ignore())
                .ForMember(dest => dest.BodyParameters, opt => opt.Ignore())
                .ForMember(dest => dest.RequestPath, opt => opt.Ignore())
                .ForMember(dest => dest.HttpMethod, opt => opt.Ignore());
        }
    }
    
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, MPartner>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));
                
            CreateMap<MPartner, Partner>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
