resource "google_compute_firewall" "firewall" {
  name          = var.firewall_config.firewall_name
  network       = var.network.name
  priority      = var.firewall_config.priority
  source_ranges = var.firewall_config.source_ranges
  allow {
    protocol = var.firewall_config.allow_rule.protocol
    ports    = var.firewall_config.allow_rule.ports
  }
}
