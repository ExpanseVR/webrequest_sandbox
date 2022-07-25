using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using UnityEngine;

public class RequestQuoteRequest : WebRequestBase
{
    [JsonProperty("orderItems")]
    public OrderItems Items { get; set; }
    [JsonProperty("userDetails")]
    public UserDetailsOrder UserDetails { get; set; }
    [JsonProperty("project")]
    public ProjectSummaryOrder Project { get; set; }

    public override void Reset()
    {
        Items = null;
        UserDetails = null;
        Project = null;
    }

    public override string UrlPath()
    {
        return "1/dpo/quote";
    }

    public override HttpMethod Method()
    {
        return HttpMethod.Post;
    }

    public override Type ResponseType()
    {
        return typeof(DefaultResponse);
    }
}

public class ProjectOrder
{
    [JsonProperty("orderItems")]
    public OrderItems OrderItems { get; set; }
        
    [JsonProperty("userDetails")]
    public UserDetailsOrder UserDetailsOrder { get; set; }

    [JsonProperty("project")]
    public ProjectSummaryOrder ProjectSummaryOrder { get; set; }
}

public class ProjectSummaryOrder
{
    [JsonProperty("name")]
    public string Name { get; set; }
        
    [JsonProperty("rooms")]
    public List<RoomSummaryOrder> RoomSummaryWrappers { get; set; }
}

public class RoomSummaryOrder
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("surfaces")]
    public List<SurfaceSummaryWrapper> Surfaces { get; set; }
}

public class SurfaceSummaryWrapper
{
    [JsonProperty("name")]
    public string Name { get; set; }
        
    [JsonProperty("measurements")]
    public MeasurementSummaryOrder MeasurementSummaryOrders { get; set; }
}

public class MeasurementSummaryOrder
{
    [JsonProperty("width")]
    public ulong Width { get; set; }
        
    [JsonProperty("height")]
    public ulong Height { get; set; }
        
    [JsonProperty("area")]
    public ulong Area { get; set; }
}

public class UserDetailsOrder
{
    [JsonProperty("email")]
    public string Email { get; set; }
        
    [JsonProperty("address")]
    public string Address { get; set; }
        
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
        
    [JsonProperty("storeName")]
    public string StoreName { get; set; }
        
    [JsonProperty("storeAbbreviation")]
    public string StoreAbbreviation { get; set; }
        
    [JsonProperty("debtorCode")]
    public string DebtorCode { get; set; }
}

public class OrderItems
{
    [JsonProperty("plasterboards")]
    public List<ItemSummaryOrder> PlasterboardSummary { get; set; }

    [JsonProperty("cornices")]
    public List<ItemSummaryOrder> CorniceSummary { get; set; }

    [JsonProperty("insulations")]
    public List<ItemSummaryOrder> InsulationSummary { get; set; }
        
    [JsonProperty("consumables")]
    public List<ConsumableItemSummaryOrder> ConsumableSummary { get; set; }
}

public class ItemSummaryOrder
{
    [JsonProperty("name")]
    public string Name { get; set; }
        
    [JsonProperty("quantity")]
    public uint Quantity { get; set; }
        
    //[JsonProperty("code")]
    //public string Code { get; set; }
        
    //[JsonProperty("totalMeasurement")]
    //public ulong TotalMeasurement { get; set; }
}

public class ConsumableItemSummaryOrder : ItemSummaryOrder
{
    [JsonProperty("auto")]
    public bool Auto { get; set; }
}

public class DefaultResponse : WebResponseBase {}