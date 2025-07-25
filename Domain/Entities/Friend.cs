using ChatApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class Friend : BaseEntity
    {
        public Guid? RequesterId { get; set; }

        public Guid? AddresseeId { get; set; }

        public FriendStatus Status { get; set; }

        [ForeignKey("AddresseeId")]
        [InverseProperty("FriendAddressees")]
        public virtual User? Addressee { get; set; }

        [ForeignKey("RequesterId")]
        [InverseProperty("FriendRequesters")]
        public virtual User? Requester { get; set; }
    }
}

