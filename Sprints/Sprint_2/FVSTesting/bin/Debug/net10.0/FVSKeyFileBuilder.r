# FVSKeyFileBuilder.r
#
# This script creates an FVS key file using rFVS
#
# Arguments from C#:
#   1: keyFileName
#   2: runTitle
#   3: standIDs (comma-separated)
#   4: inDataBase
#   5: outDataBase
#   6: ncycles
#   7: moreKeywords (newline-separated, or empty)


cat("Using rFVS\n")
library(rFVS)

cat("Getting c# arguments\n")
# Pass our key file settings from the command line
args <- commandArgs(trailingOnly = TRUE)

if (length(args) != 7) {
  stop("Expected 7 arguments from C#.")
}

key_file_name <- args[1]
run_title     <- args[2]
stand_ids     <- strsplit(args[3], ",", fixed = TRUE)[[1]]
in_database   <- args[4]
out_database  <- args[5]
ncycles       <- as.integer(args[6])

# Convert additional keywords into a character vector.
more_keywords <- if (nzchar(args[7])) {
  strsplit(args[7], "\n", fixed = TRUE)[[1]]
} else {
  NULL
}
# Verify that we have enough cycles
if (is.na(ncycles) || ncycles < 0) {
  stop("ncycles must be a positive integer.")
}
# Ensure we have an existing database
if (!file.exists(in_database)) {
  stop(paste("Input database not found:", in_database))
}


cat("Generating Key File\n")
# Generate the key file from our parameters
result <- fvsMakeKeyFile(
  keyFileName = key_file_name,
  runTitle = run_title,
  standIDs = stand_ids,
  inDataBase = in_database,
  outDataBase = out_database,
  ncycles = ncycles,
  moreKeywords = more_keywords
)

getwd()


cat("KEYFILE_CREATED\n")