resource "google_compute_network" "vpc_network" {
  name = var.network_config.vpc_name
  mtu  = 1460
}


resource "google_compute_subnetwork" "subnet" {
  for_each      = var.network_config.subnet
  name          = each.value.subnet_name
  ip_cidr_range = each.value.ip_cidr_range
  network       = google_compute_network.vpc_network.id
}


resource "google_compute_address" "address" {
  name   = "${var.network_config.vpc_name}-static-address"
  region = var.region
}


# resource "google_dns_record_set" "dns_record" {
#   name    = "minh.website"  # Replace with your DNS name
#   type    = "A"
#   ttl     = 600
#   managed_zone = "your-dns-zone-name"
#   rrdatas = ["LOAD_BALANCER_IP_ADDRESS"]  # Replace with your load balancer's IP address
# }



