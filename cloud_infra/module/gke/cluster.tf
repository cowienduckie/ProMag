resource "google_container_cluster" "cluster" {
  name                     = var.gke_config.cluster_name
  location                 = var.location
  network                  = var.network.id
  subnetwork               = var.subnet[var.gke_config.subnet_name].id
  remove_default_node_pool = var.gke_config.remove_default_node_pool
  initial_node_count       = var.gke_config.initial_node_count
  cluster_autoscaling {
    enabled = var.gke_config.autoscaling_config.enabled
    # for_each = var.gke_config.autoscaling_config.resource_limits
    resource_limits {
      resource_type = "cpu"
      minimum       = var.gke_config.autoscaling_config.resource_limits.cpu.minimum != null ? var.gke_config.autoscaling_config.resource_limits.cpu.minimum : 1
      maximum       = var.gke_config.autoscaling_config.resource_limits.cpu.maximum != null ? var.gke_config.autoscaling_config.resource_limits.cpu.maximum : 3
    }
    resource_limits {
      resource_type = "memory"
      minimum       = var.gke_config.autoscaling_config.resource_limits.memory.minimum != null ? var.gke_config.autoscaling_config.resource_limits.memory.minimum : 2
      maximum       = var.gke_config.autoscaling_config.resource_limits.memory.maximum != null ? var.gke_config.autoscaling_config.resource_limits.memory.maximum : 4
    }
  }
}


resource "google_container_node_pool" "node_pool" {
  name       = var.gke_config.node_pool_name
  location   = var.location
  cluster    = google_container_cluster.cluster.name
  node_count = var.gke_config.node_count
  node_config {
    # for_each = var.gke_config.node_config
    preemptible  = var.gke_config.node_config.preemptible != null ? var.gke_config.node_config.preemptible : true
    machine_type = var.gke_config.node_config.machine_type != null ? var.gke_config.node_config.machine_type : "n1-standard-1"
  }
}
