namespace MyApp.Api.Dto;

public record PaymentSummaryDto(
    int TotalItems,
    int ProductCostCents,
    int ShippingCostCents,
    int TotalCostBeforeTaxCents,
    int TaxCents,
    int TotalCostCents
);
