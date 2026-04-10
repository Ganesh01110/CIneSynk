terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

variable "aws_region" {
  description = "AWS region to deploy resources"
  type        = string
  default     = "us-east-1"
}

variable "project_name" {
  description = "Name of the project"
  type        = string
  default     = "cinesynk"
}

variable "environment" {
  description = "Execution environment (prod, staging, dev)"
  type        = string
  default     = "prod"
}

variable "db_password" {
  description = "MariaDB master password"
  type        = string
  sensitive   = true
}

variable "jwt_secret" {
  description = "JWT Secret for Authentication"
  type        = string
  sensitive   = true
}
