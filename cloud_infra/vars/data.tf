locals {
  sit = {
    gke_config = {
      cluster_name             = "${var.env}-cluster"
      subnet_name              = "${var.env}-subnet"
      remove_default_node_pool = true
      initial_node_count       = 1
      autoscaling_config = {
        enabled = true
        resource_limits = {
          cpu = {
            minimum = 1
            maximum = 3
          }
          memory = {
            minimum = 2
            maximum = 4
          }
        }
      }

      node_pool_name = "${var.env}-node-pool"
      node_count     = 1
      node_config = {
        preemptible  = true
        machine_type = "n1-standard-4"
      }
    }

    network_config = {
      vpc_name = "${var.env}-vpc"
      subnet = {
        # I tried cluster_subnet but it became the key defined the subnet, why?
        "${var.env}-subnet" = {
          subnet_name   = "${var.env}-subnet"
          ip_cidr_range = "10.0.0.0/24"
        }
      }
    }

    firewall_config = {
      firewall_name = "${var.env}-firewall"
      priority      = 999
      allow_rule = {
        protocol = "tcp"
        ports    = ["22", "80", "443", "8080"] # Allow SSH, HTTP, and HTTPS traffic
      }
      source_ranges = ["0.0.0.0/0"] # Allow traffic from anywhere
    }
  }

  uat  = {
    gke_config = {
      cluster_name             = "${var.env}-cluster"
      subnet_name              = "${var.env}-subnet"
      remove_default_node_pool = true
      initial_node_count       = 1
      autoscaling_config = {
        enabled = true
        resource_limits = {
          cpu = {
            minimum = 1
            maximum = 3
          }
          memory = {
            minimum = 2
            maximum = 4
          }
        }
      }

      node_pool_name = "${var.env}-node-pool"
      node_count     = 1
      node_config = {
        preemptible  = true
        machine_type = "n1-standard-4"
      }
    }

    network_config = {
      vpc_name = "${var.env}-vpc"
      subnet = {
        # I tried cluster_subnet but it became the key defined the subnet, why?
        "${var.env}-subnet" = {
          subnet_name   = "${var.env}-subnet"
          ip_cidr_range = "10.0.0.0/24"
        }
      }
    }

    firewall_config = {
      firewall_name = "${var.env}-firewall"
      priority      = 999
      allow_rule = {
        protocol = "tcp"
        ports    = ["22", "80", "443", "8080"] # Allow SSH, HTTP, and HTTPS traffic
      }
      source_ranges = ["0.0.0.0/0"] # Allow traffic from anywhere
    }
  }
  prod = {
    gke_config = {
      cluster_name             = "${var.env}-cluster"
      subnet_name              = "${var.env}-subnet"
      remove_default_node_pool = true
      initial_node_count       = 1
      autoscaling_config = {
        enabled = true
        resource_limits = {
          cpu = {
            minimum = 1
            maximum = 3
          }
          memory = {
            minimum = 2
            maximum = 4
          }
        }
      }

      node_pool_name = "${var.env}-node-pool"
      node_count     = 1
      node_config = {
        preemptible  = true
        machine_type = "n1-standard-4"
      }
    }

    network_config = {
      vpc_name = "${var.env}-vpc"
      subnet = {
        # I tried cluster_subnet but it became the key defined the subnet, why?
        "${var.env}-subnet" = {
          subnet_name   = "${var.env}-subnet"
          ip_cidr_range = "10.0.0.0/24"
        }
      }
    }

    firewall_config = {
      firewall_name = "${var.env}-firewall"
      priority      = 999
      allow_rule = {
        protocol = "tcp"
        ports    = ["22", "80", "443", "8080"] # Allow SSH, HTTP, and HTTPS traffic
      }
      source_ranges = ["0.0.0.0/0"] # Allow traffic from anywhere
    }
  }
}