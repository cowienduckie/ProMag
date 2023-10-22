terraform {
  backend "gcs" {
    bucket = "promag"
    prefix = "terraform/state"
  }
}


provider "google" {
  credentials = file(var.key_file_path)
  project     = local.global_variables.project
  region      = local.global_variables.region
}