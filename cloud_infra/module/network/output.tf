output "vpc_network" {
  value = google_compute_network.vpc_network
}

output "subnet" {
  value = google_compute_subnetwork.subnet
}

output "address" {
  value = google_compute_address.address
}