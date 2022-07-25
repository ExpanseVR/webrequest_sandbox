using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class TestRequest : MonoBehaviour
{
    private HttpClient client;

    private void Start()
    {
        client = new HttpClient();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TestConnection();
    }

    private void TestConnection()
    {
        RequestQuoteRequest request = new RequestQuoteRequest();
        
        request.Items = CreateNewOrderItems();
        
        request.Project = CreateProjecteSummary();
        
        request.UserDetails = new UserDetailsOrder
        {
            Email = "invisage13@gmail.com",
            Address = "My home address",
            PhoneNumber = "0422616900",
            StoreName = "Campbelltown",
            StoreAbbreviation = "CTN",
            DebtorCode = "52651654"
        };
        
        var contentString = JsonConvert.SerializeObject(request);
        //print(contentString);
        //return;
        
        StringContent content = new StringContent(
            contentString,
            Encoding.UTF8,
            "application/json"
        );
        
        var method = new HttpMethod("POST");
        //HttpRequestMessage webRequest = new HttpRequestMessage(method, "http://127.0.0.1:3000/1/dpo/quote");
        HttpRequestMessage webRequest = new HttpRequestMessage(method, "http://178.128.217.80:8080/1/dpo/quote");
        
        webRequest.Content = content;
        var webResponse = client.SendAsync(webRequest);
        print(webResponse.Result);
    }

    private static OrderItems CreateNewOrderItems()
    {
        var plasterOrder = new ItemSummaryOrder
        {
            Name = "This is a reallly reallly long name",
            Quantity = 20,
            //Code = "4651",
            //TotalMeasurement = 1000
        };
        
        var plasterboardSummary = new List<ItemSummaryOrder>
        {
            plasterOrder
        };
        
        var corniceOrder = new ItemSummaryOrder
        {
            Name = "This is a long name",
            Quantity = 5,
            //Code = "4631",
            //TotalMeasurement = 500
        };

        var corniceSummary = new List<ItemSummaryOrder>
        {
            corniceOrder
        };
        
        var insulationOrder = new ItemSummaryOrder
        {
            Name = "This is an even longer name",
            Quantity = 5,
            //Code = "4631",
            //TotalMeasurement = 500
        };

        var insulationSummary = new List<ItemSummaryOrder>
        {
            insulationOrder
        };

        var consumeItemOrder = new ConsumableItemSummaryOrder()
        {
            Auto = true,
            Name = "terrace",
            Quantity = 7,
            //Code = "3231",
            //TotalMeasurement = 400
        };

        var consumeSummary = new List<ConsumableItemSummaryOrder>
        {
          consumeItemOrder  
        };
        
        return new OrderItems
        {
            PlasterboardSummary = plasterboardSummary,
            CorniceSummary = corniceSummary,
            InsulationSummary = insulationSummary,
            ConsumableSummary = consumeSummary
        };
    }

    private static ProjectSummaryOrder CreateProjecteSummary()
    {
        var measureOrder = new MeasurementSummaryOrder
        {
            Width = 200,
            Height = 400,
            Area = 1630000
        };
        
        var surfaceSum = new SurfaceSummaryWrapper
        {
            Name = "Ceiling",
            MeasurementSummaryOrders = measureOrder
        };
        
        var surfaceSum2 = new SurfaceSummaryWrapper
        {
            Name = "Wall A",
            MeasurementSummaryOrders = measureOrder
        };
        
        var surfaceSumWrap1 = new List<SurfaceSummaryWrapper>
        {
            surfaceSum,
            surfaceSum2
        };

        var roomSumOrder1 = new RoomSummaryOrder
        {
            Name = "Bedroom",
            Surfaces = surfaceSumWrap1
        };

        var roomSummaries = new List<RoomSummaryOrder>
        {
            roomSumOrder1
        };
        
        return new ProjectSummaryOrder
        {
            Name = "Avengers Tower",
            RoomSummaryWrappers = roomSummaries
        };
    }
}
