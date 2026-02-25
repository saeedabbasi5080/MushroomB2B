using MediatR;

namespace MushroomB2B.Application.Features.Admin.Queries.GetDailyDeliveryReport;

public sealed record GetDailyDeliveryReportQuery(DateTime TargetDate)
    : IRequest<List<DeliveryReportItemDto>>;

public sealed record DeliveryReportItemDto(
    Guid OrderId,
    string ShopName,
    string ShopAddress,
    decimal TotalWeightKg,
    List<DeliveryReportLineDto> Items);

public sealed record DeliveryReportLineDto(
    string ProductName,
    string Grade,
    int Quantity,
    int WeightUnitKg);
