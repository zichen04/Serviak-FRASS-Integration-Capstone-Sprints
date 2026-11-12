# This temporary file automates a setup for a test enviroment for rFVS.
#
# In the future, this file will be replaced by installing the packages directly in FRASS

# r basics
install.packages("languageserver")
install.packages("RSQLite")
# so we can install packages from github
install.packages("remotes")
# the rFVS package
remotes::install_github(
  "USDAForestService/ForestVegetationSimulator-Interface",
  subdir = "rFVS"
)