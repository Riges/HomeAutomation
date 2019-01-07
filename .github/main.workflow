workflow "New workflow" {
  on = "push"
  resolves = ["Build Docker image"]
}

# Build

action "Build Docker image" {
  uses = "docker://docker:stable"
  args = ["build", "-t", "homeautomation-netatmo-query", "./src/HomeAutomation.Netatmo.Query/"]
}
