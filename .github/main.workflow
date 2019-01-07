workflow "Docker Build" {
  on = "push"
  resolves = ["Build Docker image"]
}

# Build

action "Build Docker image" {
  uses = "docker://docker:stable"
  args = "build -t homeautomation-netatmo-query -f ./src/HomeAutomation.Netatmo.Query/Dockerfile  ./src/"
}
