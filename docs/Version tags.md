# Midnattsfrost — Versioning Strategy

## Purpose

This document describes the versioning strategy for the Midnattsfrost project.

The goal is to:

- Track project progress clearly
- Create stable checkpoints
- Make collaboration easier
- Allow the team to restore earlier working versions
- Follow a professional workflow

---

# Recommended Versioning Style

Use:

```text
vMAJOR.MINOR.PATCH
```

Example:

```text
v0.1.0
```

This is based on a simplified Semantic Versioning structure.

---

# Version Structure

| Part | Meaning |
|---|---|
| MAJOR | Large breaking release |
| MINOR | New features or milestones |
| PATCH | Bug fixes and small improvements |

---

# Meaning of Each Part

## MAJOR

The first number.

Example:

```text
v1.0.0
```

Increase this when:

- The application becomes a major stable release
- Large breaking architectural changes happen

For this project:

```text
v1.0.0
```

should represent the first fully working MVP.

---

## MINOR

The middle number.

Example:

```text
v0.3.0
```

Increase this when:

- New functionality is added
- A major milestone is completed
- A new vertical slice is completed

This will be the most commonly updated number during development.

---

## PATCH

The last number.

Example:

```text
v0.3.1
```

Increase this when:

- Small bugs are fixed
- Minor improvements are added
- No major new features are introduced

---

# Recommended Midnattsfrost Progression

## Initial Setup

```text
v0.1.0
```

Meaning:

```text
Project skeleton exists
```

Suggested contents:

- Git repository
- Backend created
- Frontend created
- Basic folder structure
- Database setup started

---

## First Vertical Slice

```text
v0.2.0
```

Meaning:

```text
Frontend can save data to backend and database
```

Suggested completed features:

```text
React form
↓
ASP.NET endpoint
↓
Database save
↓
Success response
```

---

## Open-Meteo Integration

```text
v0.3.0
```

Meaning:

```text
Geocoding and forecast APIs work
```

Suggested completed features:

- Geocoding service
- Weather service
- Postman API testing
- External API integration

---

## Frost Logic Working

```text
v0.4.0
```

Meaning:

```text
App can detect frost risk
```

Suggested completed features:

- Upcoming 16-hour forecast checking
- Frost detection logic
- Frost risk endpoint

---

## Background Frost Check Prepared

```text
v0.5.0
```

Meaning:

```text
Project prepared for automatic scheduled checking
```

Suggested completed features:

- Manual frost-check endpoint
- BackgroundService planning
- Subscriber iteration logic

---

## MVP Release

```text
v1.0.0
```

Meaning:

```text
Core application is stable and demo-ready
```

Suggested completed features:

- Subscriber registration
- Geocoding
- Weather forecast
- Frost risk detection
- Stable API responses
- Error handling
- Full demo flow works

---

# Example Patch Versions

## Example

```text
v0.4.1
```

Could mean:

```text
Fixed incorrect frost threshold logic
```

---

## Another Example

```text
v0.4.2
```

Could mean:

```text
Improved weather API error handling
```

---

# Git Tagging

After important milestones, create Git tags.

Example:

```bash
git tag v0.2.0
git push origin v0.2.0
```

This creates a permanent checkpoint.

---

# Why Tags Are Important

Tags allow the team to:

```text
Restore stable versions
Compare progress
Create demo checkpoints
Avoid losing working states
```

Very useful during team projects.

---

# Recommended Team Rule

Only create version tags when:

```text
Project builds successfully
AND
Core functionality works
```

Never tag broken code.

Tags should represent stable restore points.

---

# Suggested Commit Workflow

## During Development

Use normal commits frequently.

Examples:

```text
Add subscriber entity
Add weather service
Fix frost threshold logic
Add geocoding API integration
```

---

## At Major Milestones

Create version tags.

Example flow:

```text
Many small commits
↓
Milestone completed
↓
Create version tag
```

---

# Example Timeline

| Milestone | Version |
|---|---|
| Initial skeleton | v0.1.0 |
| Database + vertical slice | v0.2.0 |
| API integration | v0.3.0 |
| Frost logic | v0.4.0 |
| Background prep | v0.5.0 |
| MVP release | v1.0.0 |

---

# Practical Recommendation

For Midnattsfrost, focus on:

```text
Frequent commits
Clear commit messages
Stable milestone tags
```

This creates a professional workflow and makes collaboration easier.

---

# Summary

Recommended strategy:

```text
Use semantic-style versioning
Start at v0.1.0
Increase MINOR for milestones
Increase PATCH for fixes
Tag stable checkpoints only
Release MVP as v1.0.0
```

This gives the project:

- Clear progress tracking
- Stable restore points
- Easier collaboration
- Professional structure
