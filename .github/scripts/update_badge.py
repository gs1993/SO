import os
import re

# Retrieve mutation score from environment variable
mutation_score = os.environ.get("MUTATION_SCORE", "N/A")

# Read README file
with open('/path/to/README.md', 'r') as readme_file:
    readme_content = readme_file.read()

# Update badge using a regular expression to match the old badge
updated_readme_content = re.sub(
    r'!\[Mutation Score\]\(https://img\.shields\.io/badge/Mutation%20Score-[0-9.]+%-blue\)',
    f'![Mutation Score](https://img.shields.io/badge/Mutation%20Score-{mutation_score}%25-blue)',
    readme_content
)

# Write back updated content to README
with open('/path/to/README.md', 'w') as readme_file:
    readme_file.write(updated_readme_content)