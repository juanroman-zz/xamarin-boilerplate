using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;

namespace Plugin.Boilerplate.Models
{
    public abstract class ModelBase : BaseNotify, IDirty
    {
        private string _id;
        private DateTime? _updatedAt;
        private DateTime? _createdAt;
        private string _version;

        [JsonProperty("id")]
        public string Id
        {
            get => _id;
            set => SetPropertyChanged(ref _id, value);
        }

        [CreatedAt]
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set => SetPropertyChanged(ref _createdAt, value);
        }

        [UpdatedAt]
        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set => SetPropertyChanged(ref _updatedAt, value);
        }

        [Version]
        public string Version
        {
            get => _version;
            set => SetPropertyChanged(ref _version, value);
        }

        [JsonIgnore]
        public bool IsDirty { get; set; }
    }
}
