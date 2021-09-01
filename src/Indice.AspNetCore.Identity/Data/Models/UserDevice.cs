﻿using System;
using Indice.Types;

namespace Indice.AspNetCore.Identity.Data.Models
{
    /// <summary>
    /// User devices representation.
    /// </summary>
    public class UserDevice
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserDevice"/> with a new <see cref="Guid"/> as Id.
        /// </summary>
        public UserDevice() : this(Guid.NewGuid()) { }

        /// <summary>
        /// Constructs a new instance of <see cref="UserDevice"/> using the given <see cref="Guid"/> as Id.
        /// </summary>
        /// <param name="id"></param>
        public UserDevice(Guid id) {
            Id = id;
        }

        /// <summary>
        /// The primary key.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Device id.
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// The user id related to this device.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Device operating system.
        /// </summary>
        public DevicePlatform DevicePlatform { get; set; }
        /// <summary>
        /// Device name.
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// The date this password was created.
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }
        /// <summary>
        /// Flag that determines if push notifications are enabled for this device.
        /// </summary>
        public bool IsPushNotificationsEnabled { get; set; }
        /// <summary>
        /// Associated password for device (when <see cref="InteractionMode"/> is equal to <see cref="InteractionMode.Pin"/>).
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Flag for pin support.
        /// </summary>
        public bool SupportsPinLogin => !string.IsNullOrWhiteSpace(Password);
        /// <summary>
        /// Device public key (when <see cref="InteractionMode"/> is equal to <see cref="InteractionMode.Fingerprint"/>).
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Flag for fingerprint support.
        /// </summary>
        public bool SupportsFingerprintLogin => !string.IsNullOrWhiteSpace(PublicKey);
        /// <summary>
        /// The user associated with this device.
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// Flag that determines if device is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// The date this device was deleted.
        /// </summary>
        public DateTimeOffset? DateDeleted { get; set; }
    }

    /// <summary>
    /// Models the way a device interacts with the identity system for trusted authorization.
    /// </summary>
    public enum InteractionMode
    {
        /// <summary>
        /// Fingerprint
        /// </summary>
        Fingerprint,
        /// <summary>
        /// 4-pin
        /// </summary>
        Pin
    }
}
