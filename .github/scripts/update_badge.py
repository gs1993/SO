import glob
import json
import re

# Search for mutation-report.json in directories under a specific path
report_files = glob.glob('/home/runner/work/SO/SO/SO/Tests/UnitTests/StrykerOutput/*/reports/mutation-report.json')

if report_files:
    # Take the first (or latest, depending on your criteria) report found
    report_file = report_files[0]
    
    with open(report_file, 'r') as f:
        data = json.load(f)
        
    mutation_score = data.get('mutationScore', None)
    
    # Read README file
    with open('/home/runner/work/SO/SO/README.md', 'r') as readme_file:
        readme_content = readme_file.read()
    
    # Update badge using a regular expression to match the old badge
    updated_readme_content = re.sub(
        r'!\[Mutation Score\]\(https://img\.shields\.io/badge/Mutation%20Score-[0-9.]+%-blue\)',
        f'![Mutation Score](https://img.shields.io/badge/Mutation%20Score-{mutation_score}%25-blue)',
        readme_content
    )
    
    # Write back updated content to README
    with open('/home/runner/work/SO/SO/README.md', 'w') as readme_file:
        readme_file.write(updated_readme_content)