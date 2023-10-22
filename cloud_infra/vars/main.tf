locals {
  env = {
    "sit" : local.sit,
    "uat" : local.uat,
    "prod" : local.prod,
  }

}

output "config" {
  value = local.env["${var.env}"]
}

output "global_variables" {
  value = local.global_variables
}