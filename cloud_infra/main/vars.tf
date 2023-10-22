module "vars" {
  source = "../vars"

  env = var.env
}

variable "env" {
  type        = string
  description = "The env to apply"
}


locals {
  config           = module.vars.config
  global_variables = module.vars.global_variables
}


variable "key_file_path" {

}
