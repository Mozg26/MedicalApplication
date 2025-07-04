﻿using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Appointments")]
    [AssignedType(typeof(AppointmentEntity))]
    public class AppointmentEntity : BaseEntity
    {
        [Column("doctor_appointment_Id")]
        public int DoctorAppointmentId { get; set; }

        [ForeignKey(nameof(DoctorAppointmentId))]
        public DoctorAppointmentEntity? DoctorAppointmentEntity { get; set; }

        [Column("appointment_type")]
        public string AppointmentType { get; set; } = string.Empty;

        [Column("appointment_date_time")]
        public DateTime AppointmentDateTime { get; set; }

        [Column("appointment_status")]
        public string AppointmentStatus { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("diagnosis")]
        public string Diagnosis {  get; set; } = string.Empty;

        [Column("prescription")]
        public string Prescription { get; set; } = string.Empty;

        [Column("cost")]
        public double Cost {  get; set; }

        [Column("cabinet")]
        public string Cabinet { get; set; } = string.Empty;

        [Column("appointment_group")]
        public string AppointmentGroup { get; set; } = string.Empty;
    }
}
