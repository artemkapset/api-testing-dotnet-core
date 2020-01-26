Feature: Simple feature for Pet shop

Scenario Outline: Getting order status

Given the following orders in the store	 
| Id		| PetId		| Quantity	| ShipDate   | Status	 | Complete |
| 361792550 | 338643049 | 1         | 2020-01-25 | Delivered | true     |
| 478563910 | 451597615 | 1         | 2020-01-31 | Approved  | false    |
| 142165722 | 438272911 | 1         | 2020-01-28 | Placed    | false    |

When i get order by '<Id>'

Then order status should be '<Status>'

Examples: 
| Id        | Status    |
| 142165722 | Placed    |
| 478563910 | Approved  |
| 361792550 | Delivered |




