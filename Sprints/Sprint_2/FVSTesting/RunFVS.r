# RunFVS.r
#
# This script builds a year-zero run for FRASS modeling
#
# Arguments from c#
#
# [0] The FVS model we are using
# [1] The bin location of the FVS Model (required by FVSRun)
# [2] The keyfile used to build the run
#
# usage RunFVS model bin keyFile
#
# Year zero runs should happen as soon as a timber cruise is submitted

library(rFVS)

# get arguments from c#
args <- commandArgs(trailingOnly = TRUE)

# validate argument count
if (length(args) < 3) {
  stop("Expected three arguments: model, bin, keyFile")
}

# Get the model and file location
model <- args[1]
bin <- args[2]

# Retrieve the keyfile
keyFile <- args[3]
# make sure the bin exists
if (!dir.exists(bin)) {
  stop(paste("FVS bin directory not found:", bin))
}
# make sure the keyfile exists
if (!file.exists(keyFile)) {
  stop(paste("Keyfile not found:", keyFile))
}

cat("Model:", model, "\n")
cat("Bin:", bin, "\n")
cat("Keyfile:", normalizePath(keyFile), "\n")

# Load an FVS Model
fvsLoad(model, bin)

# Send the keyfile to fvs
fvsSetCmdLine(
    paste0("--keywordfile=", normalizePath(keyFile))
)

# Run FVS
fvsRun()

# Get summery for c# and testing
# fvsGetSummery()