module "network" {
  source = "../module/network"

  network_config = local.config.network_config
  region         = local.global_variables.region
}

module "gke" {
  source = "../module/gke"

  gke_config = local.config.gke_config
  network    = module.network.vpc_network
  subnet     = module.network.subnet
  location   = local.global_variables.location
}

module "security" {
  source = "../module/security"

  network         = module.network.vpc_network
  firewall_config = local.config.firewall_config
}
