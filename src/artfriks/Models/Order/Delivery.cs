using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using artfriks.Models;
using System.ComponentModel.DataAnnotations;

namespace artfriks.Models
{
    public class Delivery
    {
    }
    public class DeliveryDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DeliveryAssignedTo { get; set; }
        public DateTime DeliveryAssignedTime { get; set; }
        public string DeliveryAssignedBy { get; set; }
        public int DeliveryStatus { get; set; }
        public int Active { get; set; }
    }

    public class Feedback
    {
        public int id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }


    public class CartId
    {
        public int id { get; set; }
        public string productType { get; set; }
        public int productUnit { get; set; }
        public string cartId { get; set; }
    }

    public class DailyDiscount
    {
        public int Id { get; set; }
        public int ArtId { get; set; }
        public int CategoryId { get; set; }
        public int DiscountType { get; set; }
        public int DiscountAmount { get; set; }
        public int DiscountPercent { get; set; }
    }

    public class OrderTransaction
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public float TransactionAmount { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string TransactionRequestString { get; set; }
        public string TransactionResponseString { get; set; }
        public string TransactionSignature { get; set; }
        public string ModeOfPayment { get; set; }
        public string CartId { get; set; }
        public string IsRefund { get; set; }
        public DateTime Adddate { get; set; }
        public OrderTransaction() { this.Adddate = DateTime.Now; }
    }


    public class ArtReview
    {
        public int Id { get; set; }
        public string ArtId { get; set; }
        public string Review { get; set; }
        public string ApplicationUser { get; set; }
        public int StarRating { get; set; }
        public DateTime Adddate { get; set; }
        public int active { get; set; }
    }


    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Invoice() { this.InvoiceDate = DateTime.Now; }
        public string InvoiceNote { get; set; }
        public string TaxRate { get; set; }
        public string TaxAmount { get; set; }
        public int DiscountPercentage { get; set; }
        public float DiscountAmount { get; set; }
        public float GrandTotal { get; set; }
        public float ShippingAmount { get; set; }
        public string AppliedCoupen { get; set; }
        public float TotalPayableAmount { get; set; }
        public int Active { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string CartId { get; set; }
        public int DiscountPercentage { get; set; }
        public string OrderMessage { get; set; }
        public int DeliveryAddressId { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal OriginalAmount { get; set; }
        public int Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Order()
        {
            OrderDate = DateTime.Now;
            Active = 0;
            IsCoupenApplied = false; IsTokenApplied = false;
        }
        public bool IsTokenApplied { get; set; }
        public bool IsCoupenApplied { get; set; }
        public int Active { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductType { get; set; }
        public int ProductUnit { get; set; }
        public DateTime DateCreated { get; set; }
        public Decimal totalPrice { get; set; }
        public virtual ArtWork Product { get; set; }
        public virtual Order Order { get; set; }
    }


    public class Notification
    {
        public int Id { get; set; }
        public DateTime Adddate { get; set; }
        public Notification() { Adddate = DateTime.Now; }
        public string ApplicationUserId { get; set; }
        public string NoteMessage { get; set; }
        public string NoteUrl { get; set; }
        public bool Read { get; set; }
        public string Icon { get; set; }
    }

    public class Invite
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }


    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string UserId { get; set; }
        public string ProductType { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductUnit { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public CartItem() { this.DateCreated = DateTime.Now; }
        public Decimal totalPrice { get; set; }
        public virtual ArtWork Product { get; set; }
    }

    public class OrderItemViewModel
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductType { get; set; }
        public DateTime DateCreated { get; set; }
        public Decimal totalPrice { get; set; }
        public virtual ArtWork Product { get; set; }
        public string ItemPicture { get; set; }
    }


    public class DeliveryBoys
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public DateTime Adddate { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
    }
}
