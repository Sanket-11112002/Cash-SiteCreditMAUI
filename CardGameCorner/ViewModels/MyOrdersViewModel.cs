﻿using CardGameCorner.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels;

public partial class MyOrdersViewModel : ObservableObject
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISecureStorage _secureStorage;

    [ObservableProperty]
    private ObservableCollection<OrderModel> orders;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage;


    [ObservableProperty]
    private bool showEmptyMessage;

    [ObservableProperty]
    private string emptyMessage = "You do not have any orders!";

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasOrders => Orders?.Count > 0;

    public MyOrdersViewModel(IHttpClientFactory httpClientFactory, ISecureStorage secureStorage)
    {
        _httpClientFactory = httpClientFactory;
        _secureStorage = secureStorage;
        Orders = new ObservableCollection<OrderModel>();
    }

    public async Task LoadOrdersAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            ShowEmptyMessage = false;

            var token = await _secureStorage.GetAsync("jwt_token");
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Not authorized. Please log in.";
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("https://api.magiccorner.it/api/mcBuylistMyOrders");

            if (response.IsSuccessStatusCode)
            {
                var ordersList = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                Orders.Clear();

                if (ordersList != null && ordersList.Any())
                {
                    foreach (var order in ordersList)
                    {
                        Orders.Add(order);
                    }
                    ShowEmptyMessage = false;
                }
                else
                {
                    ShowEmptyMessage = true;
                }
            }
            else
            {
                ErrorMessage = "Failed to load orders. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred while loading orders.";
            Debug.WriteLine($"Error loading orders: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(HasOrders));
        }
    }
}